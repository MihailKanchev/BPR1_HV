/*
 * RTOS_driver.c
 *
 * Created: 12/12/2020 2:05:45 PM
 * Author: Dominika Kubicz
 * Project: Herning Vand Reduction of Supervision
 */ 

#include "itimer_driver.h"
#include <ATMEGA_FreeRTOS.h>
#include <timers.h>

TimerHandle_t TimeoutTimer;
bool timeout_flag;

void update_flag()
{
	timeout_flag = true;
}

bool getTimeout()
{
	return timeout_flag;
}

void consumeFlag()
{
	timeout_flag = false;
}

void intialize_timer()
{
	timeout_flag = true;
	
	TimeoutTimer = xTimerCreate( "Timeout Timer",
                     /* The timer period in ticks, must be
                     greater than 0. */
                     5000,
                     /* The timers will auto-reload themselves
                     when they expire. */
                     pdTRUE,
                     /* The ID is used to store a count of the
                     number of times the timer has expired, which
                     is initialised to 0. */
                     ( void * ) 0,
                     /* Each timer calls the same callback when
                     it expires. */
                     update_flag
                   );
				   
	xTimerStart(TimeoutTimer, 0);
}