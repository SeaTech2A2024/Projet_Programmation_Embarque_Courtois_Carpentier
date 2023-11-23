#ifndef PWM_H
#define PWM_H

#define MOTEUR_DROIT 0
#define MOTEUR_GAUCHE 1



#include <xc.h> // Bibliothèque xc.h inclut tous les uC
#include "IO.h"
#include "Robot.h"
#include "ToolBox.h"

#define PWMPER 40.0

extern float acceleration;

void InitPWM(void);
//void PWMSetSpeed(float vitesseEnPourcent, int numeroMoteur);
void PWMSetSpeedConsigne(float vitesseEnPourcents, int moteur);
void PWMUpdateSpeed();

#endif // PWM_H
