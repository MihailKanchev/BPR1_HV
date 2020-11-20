/*
 * ADC_driver.c
 *
 * Created: 11/18/2020 3:27:55 PM
 * Author: Dominika Kubicz
 * Project: Herning Vand Reduction of Supervision
 */ 

#include "iADC_driver.h"
#include <stdio.h>
#include <avr/io.h>
#include <avr/interrupt.h>
#include <display_7seg.h>

static uint16_t result;

void init_adc()
{
	//Set PK0 as input
	DDRK &= ~(_BV(PK0));
	//Choosing ADC8 channel (100000);
	ADMUX |= _BV(REFS0);
	ADCSRB |= _BV(MUX5);
	/** Enable ADC (ADEN), set free running mode (ADATE)
	enable interrupt (ADIE), set prescaler to 128**/
	ADCSRA |= _BV(ADEN) | _BV(ADIE) | _BV(ADPS0) | _BV(ADPS1) | _BV(ADPS2);

	
	//start conversion
	ADCSRA |= _BV(ADSC);
	printf("Conversion started");
}

//reading the converted value
ISR(ADC_vect)
{
	result = ADC;
	printf("Value converted to %d", result);
	display_7seg_display(result,0);
}
