/*
 * main.c
 *
 * Created: 11/18/2020 3:11:46 PM
 * Author: Dominika Kubicz
 * Project: Herning Vand Reduction of Supervision
*/

#include <stdio.h>
#include <avr/io.h>
#include <avr/sfr_defs.h>
#include <avr/interrupt.h>
#include "ADC/iADC_driver.h"
#include "Temperature_sensor/itemperature.h"
#include "Pressure_sensor/ipressure.h"
#include "reading_struct.h"

// Drivers
#include <display_7seg.h>

#include <ATMEGA_FreeRTOS.h>
#include <semphr.h>

#include <FreeRTOSTraceDriver.h>
#include <stdio_driver.h>
#include <serial.h>

// define three Tasks
void temperature( void *pvParameters );
void pressure( void *pvParameters );
void lorawan( void *pvParameters );

// define semaphore handle
SemaphoreHandle_t xTestSemaphore;

//A queue for data
QueueHandle_t xQueue;

/*-----------------------------------------------------------*/
void create_tasks_and_semaphores(void)
{
	if ( xTestSemaphore == NULL )  // Check to confirm that the Semaphore has not already been created.
	{
		xTestSemaphore = xSemaphoreCreateMutex();  // Create a mutex semaphore.
		if ( ( xTestSemaphore ) != NULL )
		{
			xSemaphoreGive( ( xTestSemaphore ) );  // Make the mutex available for use, by initially "Giving" the Semaphore.
		}
	}
	
	//creating a queue that can hold two items of size uint16
	xQueue = xQueueCreate(2,sizeof(struct reading));

	xTaskCreate(
	temperature
	,  (const portCHAR *)"temperature_sensor"  // A name just for humans
	,  configMINIMAL_STACK_SIZE  // This stack size can be checked & adjusted by reading the Stack Highwater
	,  NULL
	,  2  // Priority, with 3 (configMAX_PRIORITIES - 1) being the highest, and 0 being the lowest.
	,  NULL );

	xTaskCreate(
	pressure
	,  (const portCHAR *)"pressure_sensor"  // A name just for humans
	,  configMINIMAL_STACK_SIZE  // This stack size can be checked & adjusted by reading the Stack Highwater
	,  NULL
	,  2  // Priority, with 3 (configMAX_PRIORITIES - 1) being the highest, and 0 being the lowest.
	,  NULL );
	
	xTaskCreate(
	lorawan
	,  (const portCHAR *)"lorawan_handler"  // A name just for humans
	,  configMINIMAL_STACK_SIZE  // This stack size can be checked & adjusted by reading the Stack Highwater
	,  NULL
	,  3  // Priority, with 3 (configMAX_PRIORITIES - 1) being the highest, and 0 being the lowest.
	,  NULL );
}

/*-----------------------------------------------------------*/
void temperature( void *pvParameters )
{
	initialize_temp(&xQueue);
	while(1){
		measure_temp();
		vTaskDelay(6000/portTICK_PERIOD_MS);
	}
}

/*-----------------------------------------------------------*/
void pressure( void *pvParameters )
{
	initialize_pressure(&xQueue);
	while(1){
		vTaskDelay(3000/portTICK_PERIOD_MS);
		measure_pressure();
		vTaskDelay(3000/portTICK_PERIOD_MS);
	}
}

/*-----------------------------------------------------------*/
void lorawan( void *pvParameters )
{
	struct reading received_reading;
	while(1){
		if(xQueueReceive(xQueue, (void*)&received_reading, portMAX_DELAY)){			
		
			float value = received_reading.value;
			uint16_t label = received_reading.readingLabel;
			if(label==0){
				printf("Received TEMPERATURE with value: %f\n", value);
			} else if(label==1){
				printf("Received PRESSURE with value: %.1f\n", value);
			}	else {
				printf("Unrecognized label\n");
			}
			display_7seg_display(value, 2);
			} else {
			printf("Error when reading from queue\n");
		}
		vTaskDelay(2000/portTICK_PERIOD_MS);
	}
}

/*-----------------------------------------------------------*/
void initialiseSystem()
{	
	// Make it possible to use stdio on COM port 0 (USB) on Arduino board - Setting 57600,8,N,1
	stdioCreate(ser_USART0);
	
	// Create some tasks
	create_tasks_and_semaphores();
	
	// Initialize drivers
	display_7seg_init(NULL);
	display_7seg_power_up();
	
	//enable interrupts
	sei();
	
	//start ADC
	init_adc();
}

/*-----------------------------------------------------------*/
int main(void)
{	initialiseSystem(); // Must be done as the very first thing!!
	printf("Program Started!!\n");
	vTaskStartScheduler(); // Initialise and run the freeRTOS scheduler. Execution should never return from here.

	/* Replace with your application code */
	while (1)
	{
	}
}