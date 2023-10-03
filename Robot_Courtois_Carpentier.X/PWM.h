#ifndef PWM_H
#define PWM_H

#include <xc.h> // Bibliothèque xc.h inclut tous les uC
#include "IO.h"
#include "Robot.h"
#include "ToolBox.h"

#define PWMPER 40.0

extern unsigned char acceleration;

void InitPWM(void);
void PWMSetSpeed(float vitesseEnPourcents);

#endif // PWM_H
