/*
 * temperature.c
 *
 * Created: 11/25/2020 12:33:07 PM
 * Author: Dominika Kubicz
 * Project: Herning Vand Reduction of Supervision
 */ 

#include "../ADC/iADC_driver.h"
#include "../reading_struct.h"

#define TEMP_LABEL 0
#define SLOPE 0.02110582016051
#define INTERSECTION -3.0671629684

QueueHandle_t xQueue;
int adc_read;
float temp;

void initialize_temp(QueueHandle_t *ptrQueue)
{
	xQueue = *(QueueHandle_t*)ptrQueue;
	adc_read = -1;
	temp = 0;
}

void measure_temp()
{
	start_temp_read(&adc_read);
	
	//wait for the ADC to read the value
	while(adc_read==-1){
		;
	}
	convert_temp();
}

void convert_temp()
{
	struct reading measurmentTemp;
	
	temp = (float) adc_read*SLOPE+INTERSECTION;
	
	measurmentTemp.readingLabel = TEMP_LABEL;
	measurmentTemp.value = (float) temp;
	
	if(!xQueueSend(xQueue, (void*)&measurmentTemp, 100)) {
		printf("Failed to send temperature to the queue\n");
	} else {
		printf("Temperature added to queue\n");
	};
	
	//reset value
	adc_read = -1;
}