/*
 * LoRa_driver.c
 *
 * Created: 12/2/2020 10:01:22 AM
 * Author: Dominika Kubicz
 * Project: Herning Vand Reduction of Supervision
 */ 

#include "../reading_struct.h"
#include "iLoRa_driver.h"
#include <ihal.h>
#include <lora_driver.h>

#define LED_TASK_PRIORITY 7

lora_payload_t uplinkPayload;
QueueHandle_t xQueue;

void initialize_lora(QueueHandle_t *ptrQueue){
	xQueue = *(QueueHandle_t*)ptrQueue;
	
	hal_create(LED_TASK_PRIORITY); // Must be called first!! LED_TASK_PRIORITY must be a high priority in your system
	lora_driver_create(ser_USART1, NULL); // The parameter is the USART port the RN2483 module is connected to - in this case USART1 - here no message buffer for down-link messages are defined

}

void send_measurements(){
	
}