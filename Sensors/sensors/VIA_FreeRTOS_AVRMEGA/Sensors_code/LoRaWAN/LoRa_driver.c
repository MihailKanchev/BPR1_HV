/*
 * LoRa_driver.c
 *
 * Created: 12/2/2020 10:01:22 AM
 * Author: Dominika Kubicz
 * Project: Herning Vand Reduction of Supervision
 */ 

#include <ihal.h>
#include <lora_driver.h>
#include "../reading_struct.h"

// Parameters for OTAA join
#define LORA_appEUI "78CFC104DD488A92"
#define LORA_appKEY "0B05BDE814D025498B896DBA0A1CCC47"
#define LORA_devEUI "0004A30B0025A3D5"