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
//#include <queue.h>

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
	xQueue = xQueueCreate(2,sizeof(uint16_t));

	xTaskCreate(
	temperature
	,  (const portCHAR *)"temperature_sensor"  // A name just for humans
	,  configMINIMAL_STACK_SIZE  // This stack size can be checked & adjusted by reading the Stack Highwater
	,  NULL
	,  3  // Priority, with 3 (configMAX_PRIORITIES - 1) being the highest, and 0 being the lowest.
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
	,  1  // Priority, with 3 (configMAX_PRIORITIES - 1) being the highest, and 0 being the lowest.
	,  NULL );
}

/*-----------------------------------------------------------*/
void temperature( void *pvParameters )
{
	while(1){
		get_reading(&xQueue);
		uint16_t value;
		if(xQueueReceive(xQueue,  &(value), portMAX_DELAY)){
			printf("Value received in main: %d\n", value);
		} else {
			printf("Error when reading the queue\n");
		}
		vTaskDelay(1000/portTICK_PERIOD_MS);
	}
}

/*-----------------------------------------------------------*/
void pressure( void *pvParameters )
{
	while(1){
		vTaskDelay(10000/portTICK_PERIOD_MS);
	}
}

/*-----------------------------------------------------------*/
void lorawan( void *pvParameters )
{
	while(1){
		vTaskDelay(10000);
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