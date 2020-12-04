/*
 * itemperature.h
 *
 * Created: 11/25/2020 12:32:48 PM
 * Author: Dominika Kubicz
 * Project: Herning Vand Reduction of Supervision
 */ 


#ifndef ITEMPERATURE_H_
#define ITEMPERATURE_H_

void initialize_temp(QueueHandle_t *ptrQueue);
void measure_temp();
void convert_temp();


#endif /* ITEMPERATURE_H_ */