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

#define LED_TASK_PRIORITY 5

float temp;
float press;
QueueHandle_t xQueue;
struct reading temporary;


void initialize_lora(QueueHandle_t *ptrQueue){
	printf("Initializing\n");
	xQueue = *(QueueHandle_t*)ptrQueue;
	temp = 0;
	press = 0;
	
}

void lora_join(){
	
	lora_driver_returnCode_t rc;
	// Join the LoRaWAN
	uint8_t maxJoinTriesLeft = 10;
	
	do {
		rc = lora_driver_join(LORA_OTAA);
		printf("Join Network TriesLeft:%d >%s<\n", maxJoinTriesLeft, lora_driver_mapReturnCodeToText(rc));

		if ( rc != LORA_ACCEPTED)
		{
			// Wait 5 sec and lets try again
			vTaskDelay(pdMS_TO_TICKS(5000UL));
		}
		else
		{
			break;
		}
	} while (rc != LORA_ACCEPTED);
	
}

void send_measurements(){
	
	// read two queue entries
	//for(int i=0; i<2;i++) {
		//if(xQueueReceive(xQueue, (void*)&temporary, 5)){
			//if(temporary.readingLabel==0){
				//temp = temporary.value;
				//printf("Adding to payload temperature: %f", temp);
			//} else if (temporary.readingLabel==1){
				//press = temporary.value;
				//printf("Adding to payload pressure: %f", press);
			//}
			//} else {
			//printf("Something went wrong while getting the data from the queue\n");
		//}
	//}
	
	// put data in the payload
	lora_driver_payload_t uplinkPayload;
	
	uplinkPayload.len = 4; // Length of the actual payload
	uplinkPayload.port_no = 2; // The LoRaWAN port no to sent the message to
	
	uplinkPayload.bytes[0] = 0b00000011;
	uplinkPayload.bytes[1] = 0b10101010;
	uplinkPayload.bytes[2] = 0b00000010;
	uplinkPayload.bytes[3] = 0b11110000;
	
	printf("Upload Message >%s<\n", lora_driver_mapReturnCodeToText(lora_driver_sendUploadMessage(false, &uplinkPayload)));
}