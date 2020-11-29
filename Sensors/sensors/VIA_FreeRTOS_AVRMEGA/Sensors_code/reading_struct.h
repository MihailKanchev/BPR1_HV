/*
 * reading_struct.h
 *
 * Created: 11/25/2020 2:26:51 PM
 * Author: Dominika Kubicz
 * Project: Herning Vand Reduction of Supervision
 */ 


#ifndef READING_STRUCT_H_
#define READING_STRUCT_H_


struct reading {
	uint8_t readingLabel; // e.g. 0 - temp, 1 - pressure
	float value; // the converted value read by the sensor
};


#endif /* READING_STRUCT_H_ */