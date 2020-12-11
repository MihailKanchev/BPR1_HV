/*
* main.c
*
* Created: 11/18/2020 3:11:46 PM
* Author: Dominika Kubicz
* Project: Herning Vand Reduction of Supervision
*/

#include <ATMEGA_FreeRTOS.h>
#include <stddef.h>
#include <status_leds.h>
#include <semphr.h>
#include <FreeRTOSTraceDriver.h>
#include <stdio_driver.h>
#include <serial.h>
#include <avr/io.h>
#include <avr/sfr_defs.h>
#include <avr/interrupt.h>
#include <ihal.h>
#include <lora_driver.h>
#include "LoRaWAN/iLoRa_driver.h"
#include "ADC/iADC_driver.h"
#include "Temperature_sensor/itemperature.h"
#include "Pressure_sensor/ipressure.h"
#include "reading_struct.h"

// Drivers
#include <display_7seg.h>

// Parameters for OTAA join
#define LORA_appEUI "78CFC104DD488A92"
#define LORA_appKEY "0B05BDE814D025498B896DBA0A1CCC47"

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
	//initialize_temp(&xQueue);
	while(1){
		vTaskDelay(10000/portTICK_PERIOD_MS);
		//measure_temp();
		//vTaskDelay(6000/portTICK_PERIOD_MS);
	}
}

/*-----------------------------------------------------------*/
void pressure( void *pvParameters )
{
	//initialize_pressure(&xQueue);
	while(1){
		vTaskDelay(10000/portTICK_PERIOD_MS);
		//vTaskDelay(3000/portTICK_PERIOD_MS);
		//measure_pressure();
		//vTaskDelay(3000/portTICK_PERIOD_MS);
	}
}

/*-----------------------------------------------------------*/
void lorawan( void *pvParameters )
{
	initialize_lora(&xQueue);

	// Hardware reset of LoRaWAN transceiver
	lora_driver_resetRn2483(1);
	vTaskDelay(2);
	lora_driver_resetRn2483(0);
	// Give it a chance to wakeup
	vTaskDelay(150);

	lora_driver_flushBuffers(); // get rid of first version string from module after reset!

	// Factory reset the transceiver
	printf("FactoryReset >%s<\n", lora_driver_mapReturnCodeToText(lora_driver_rn2483FactoryReset()));
	
	// Configure to EU868 LoRaWAN standards
	printf("Configure to EU868 >%s<\n", lora_driver_mapReturnCodeToText(lora_driver_configureToEu868()));

	// Get the RN2483 modules unique devEUI
	static char devEui[17]; // It is static to avoid it to occupy stack space in the task
	if (lora_driver_getRn2483Hweui(devEui) != LORA_OK)
	{
		printf("Error when receiving device EUI!\n");
	}
	
	// Set the necessary LoRaWAN parameters for an OTAA join
	if (lora_driver_setOtaaIdentity(LORA_appEUI,LORA_appKEY,devEui) != LORA_OK)
	{
		printf("Error when setting OTAA parameters!\n");
	}
	
	// Enable Adaptive Data Rate
	printf("Set Adaptive Data Rate: ON >%s<\n", lora_driver_mapReturnCodeToText(lora_driver_setAdaptiveDataRate(LORA_ON)));

	
	while(1){
		
		//if (uxQueueSpacesAvailable(xQueue) == 0)
			lora_join();
		//}
		vTaskDelay(30000/portTICK_PERIOD_MS);
		
	}
}

/*-----------------------------------------------------------*/
void initialiseSystem()
{
	// Make it possible to use stdio on COM port 0 (USB) on Arduino board - Setting 57600,8,N,1
	stdio_create(ser_USART0);
	
	hal_create(5); // Must be called first!! LED_TASK_PRIORITY must be a high priority in your system
	lora_driver_create(1, NULL); // The parameter is the USART port the RN2483 module is connected to - in this case USART1 - here no message buffer for down-link messages are defined

	
	// Create some tasks
	create_tasks_and_semaphores();
	
	// Initialize drivers
	display_7seg_init(NULL);
	display_7seg_powerUp();
	
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