/*
 * pressure.c
 *
 * Created: 11/25/2020 12:35:09 PM
 * Author: Dominika Kubicz
 * Project: Herning Vand Reduction of Supervision
 */ 

#include "../ADC/iADC_driver.h"
#include "../reading_struct.h"

#define PRESS_LABEL 1
#define VOLTAGE_SUPPLY 5
#define PRECISION 1024

QueueHandle_t xQueue;
int adc_reading;
float press;
float volts;
float pascal;
float bars;

void initialize_pressure(QueueHandle_t *ptrQueue)
{
	xQueue = *(QueueHandle_t*)ptrQueue;
	adc_reading = 0;
	press = -1;
	volts = 0;
	pascal = 0;
	bars = 0;
}

void measure_pressure()
{
	start_pressure_read(&adc_reading);
	
	//wait for the ADC to read the value
	while(adc_reading==-1){
		;
	}
	convert_pressure();
}

void convert_pressure()
{
	struct reading measurmentPress;
	
	volts = (float)adc_reading*VOLTAGE_SUPPLY/PRECISION;
	pascal = (float) (3*(volts-0.44))*1000000;
	bars = pascal/100000;
	printf("Bars: %.3f\n", bars);
	measurmentPress.readingLabel = PRESS_LABEL;
	measurmentPress.value = bars;
	if(!xQueueSend(xQueue, (void*)&measurmentPress, 100)) {
		printf("Failed to send pressure to the queue\n");
		} else {
		printf("Pressure added to queue\n");
		vTaskDelay(100); // let other sensors write their data to the queue
	};
	adc_reading = -1;
}