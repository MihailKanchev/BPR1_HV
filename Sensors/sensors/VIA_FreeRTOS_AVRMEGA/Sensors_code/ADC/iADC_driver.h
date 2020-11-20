/*
 * iADC_driver.h
 *
 * Created: 11/18/2020 3:26:23 PM
 * Author: Dominika Kubicz
 * Project: Herning Vand Reduction of Supervision
 */ 

#include <ATMEGA_FreeRTOS.h>
#include <semphr.h>

#ifndef IADC_DRIVER_H_
#define IADC_DRIVER_H_

void init_adc();
void get_reading(QueueHandle_t *queue);



#endif /* IADC_DRIVER_H_ */