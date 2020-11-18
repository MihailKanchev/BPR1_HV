/*
* main.c
* Author : LBS

System tick = 58 ticks pr. second
*/

#include <stdio.h>
#include <avr/io.h>
#include <avr/sfr_defs.h>

// Drivers
#include <rcServo.h>
#include <display_7seg.h>
#include <hih8120.h>

#include <ATMEGA_FreeRTOS.h>
#include <semphr.h>

#include <FreeRTOSTraceDriver.h>
#include <stdio_driver.h>
#include <serial.h>


// define two Tasks
void task1( void *pvParameters );
void task2( void *pvParameters );

// define semaphore handle
SemaphoreHandle_t xTestSemaphore;

// Global variables
uint8_t hih8120CreatedOK;

/*-----------------------------------------------------------*/
void create_tasks_and_semaphores(void)
{
	// Semaphores are useful to stop a Task proceeding, where it should be paused to wait,
	// because it is sharing a resource, such as the Serial port.
	// Semaphores should only be used whilst the scheduler is running, but we can set it up here.
	if ( xTestSemaphore == NULL )  // Check to confirm that the Semaphore has not already been created.
	{
		xTestSemaphore = xSemaphoreCreateMutex();  // Create a mutex semaphore.
		if ( ( xTestSemaphore ) != NULL )
		{
			xSemaphoreGive( ( xTestSemaphore ) );  // Make the mutex available for use, by initially "Giving" the Semaphore.
		}
	}

	xTaskCreate(
	task1
	,  (const portCHAR *)"Task1"  // A name just for humans
	,  configMINIMAL_STACK_SIZE  // This stack size can be checked & adjusted by reading the Stack Highwater
	,  NULL
	,  2  // Priority, with 3 (configMAX_PRIORITIES - 1) being the highest, and 0 being the lowest.
	,  NULL );

	xTaskCreate(
	task2
	,  (const portCHAR *)"Task2"  // A name just for humans
	,  configMINIMAL_STACK_SIZE  // This stack size can be checked & adjusted by reading the Stack Highwater
	,  NULL
	,  1  // Priority, with 3 (configMAX_PRIORITIES - 1) being the highest, and 0 being the lowest.
	,  NULL );
}

/*-----------------------------------------------------------*/
void task1( void *pvParameters )
{
	#if (configUSE_APPLICATION_TASK_TAG == 1)
	// Set task no to be used for tracing with R2R-Network
	vTaskSetApplicationTaskTag( NULL, ( void * ) 1 );
	#endif

	TickType_t xLastWakeTime;
	const TickType_t xFrequency = 58; // 1s
	
	// Initialize the xLastWakeTime variable with the current time.
	xLastWakeTime = xTaskGetTickCount();
	int8_t percent =-100;
	uint8_t up = 1;
	
	
	for(;;)
	{
		vTaskDelayUntil( &xLastWakeTime, xFrequency );
		hih8120Test();
		
		// test display
		display_7seg_display(100+percent, 1);
		
		// test I/O   -   stdio functions are not reentrant - Should normally be protected by MUTEX
		if(stdioInputWaiting()){
			int i = getchar();
			printf("\n   Input received: %c \n\n", i);
		}
		
		// test rc servo
		if(up){
			rcServoSet (0,  percent);
			percent=percent+10;
			if(percent>100){
				up = 0;
				percent =100;
			}
		}
		else{
			rcServoSet (0,  percent);
			percent=percent-10;
			if(percent<-100){
				up = 1;
				percent =-100;
			}
		}
		
	}
}

/*-----------------------------------------------------------*/
void task2( void *pvParameters )
{
	#if (configUSE_APPLICATION_TASK_TAG == 1)
	// Set task no to be used for tracing with R2R-Network
	vTaskSetApplicationTaskTag( NULL, ( void * ) 2 );
	#endif

	TickType_t xLastWakeTime;
	const TickType_t xFrequency = 6; // 100 ms

	// Initialize the xLastWakeTime variable with the current time.
	xLastWakeTime = xTaskGetTickCount();
	
	for(;;)
	{
		vTaskDelayUntil( &xLastWakeTime, xFrequency );
		
		uint8_t sw0 = PINC & _BV(PINC0);
		
		PORTA = ((PIND & (_BV(PIND3) | _BV(PIND2))) << 4) | (PINC & (_BV(PINC0) | _BV(PINC1)| _BV(PINC2) | _BV(PINC3) | _BV(PINC4)  | _BV(PINC5)));
		
		// stdio functions are not reentrant - Should normally be protected by MUTEX
		//printf("\n\n\nsw0 %d",((PIND & (_BV(PIND3) | _BV(PIND2))) << 4) | (PINC & (_BV(PINC0) | _BV(PINC1)| _BV(PINC2) | _BV(PINC3) | _BV(PINC4) | _BV(PINC5))));
		
	}
}

/*-----------------------------------------------------------*/
void initialiseSystem()
{
	// Set output ports for leds used in the example
	// Set PORTA to output
	DDRA = 0xFF;
	
	// Set PORTD pin 2, 3 and PORTC 0-5 to pull up on input
	PORTD |= _BV(PIND2) | _BV(PIND3);
	PORTC |= _BV(PINC0) | _BV(PINC1)| _BV(PINC2) | _BV(PINC3) | _BV(PINC4) | _BV(PINC5);
	
	// Make it possible to use stdio on COM port 0 (USB) on Arduino board - Setting 57600,8,N,1
	stdioCreate(ser_USART0);
	
	// Create some tasks
	create_tasks_and_semaphores();
	
	// Initialize drivers
	
	display_7seg_init(NULL);
	display_7seg_power_up();
	
	rcServoCreate ();
	
	
	// Always check what hih8120Create() returns
	if ( HIH8120_OK == hih8120Create() )
	{
		// Driver created OK
		hih8120CreatedOK = true;
	}
	else{
		// Else driver not created
		hih8120CreatedOK = false;
	}
	
	
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

void hih8120Test(){
	uint16_t humidity = 0;
	int16_t temperature = 0;
	
	//	hih8120 status
	if(hih8120CreatedOK){
		if (HIH8120_OK == hih8120Wakeup()) {
			vTaskDelay(100/portTICK_PERIOD_MS);
			
			if (HIH8120_OK ==  hih8120Meassure()){
				vTaskDelay(10/portTICK_PERIOD_MS);
				humidity = hih8120GetHumidityPercent_x10();
				temperature = hih8120GetTemperature_x10();
			}
			else {
				// Something went wrong
				printf("\n hih8120 did not measure\n");
			}
		}
		else {
			// Something went wrong
			printf("\n hih8120 did not wakeup\n");
		}
	}
	else{
		// Something went wrong
		printf("\n hih8120 not created");
	}
	
	printf("\n humidity: %d\n", humidity);
	printf("\n temperature: %d\n", temperature);
}

