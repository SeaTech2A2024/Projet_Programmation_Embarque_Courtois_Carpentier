#include <stdio.h>
#include <stdlib.h>
#include <xc.h>
#include "ChipConfig.h"
#include  "IO.h"
#include "timer.h"
#include "Robot.h"
#include "PWM.h"
#include "adc.h"
#include "main.h"

unsigned char stateRobot;

int ADCValue0;
int ADCValue1;
int ADCValue2;
int ADCValue3;
int ADCValue4;

int main(void) {
    /***************************************************************************************************/
    //Initialisation de l?oscillateur
    /****************************************************************************************************/
    InitOscillator();

    /****************************************************************************************************/
    // Configuration des entrées sorties
    /****************************************************************************************************/
    InitIO();

    //initialisation du timer23
    InitTimer23();
    //initialisation du timer1
    InitTimer1();

    // initialisation des moteur
    InitPWM();
    // initialiser ADC
    InitADC1();

    /****************************************************************************************************/
    // Boucle Principale
    /****************************************************************************************************/
    while (1) {
        if (ADCIsConversionFinished() == 1) {
            ADCClearConversionFinishedFlag();
            unsigned int * result = ADCGetResult();
            float volts = ((float) result [1])* 3.3 / 4096 * 3.2;
            robotState.distanceTelemetreDroit = 34 / volts - 5;
            volts = ((float) result [2])* 3.3 / 4096 * 3.2;
            robotState.distanceTelemetreCentre = 34 / volts - 5;
            volts = ((float) result [4])* 3.3 / 4096 * 3.2;
            robotState.distanceTelemetreGauche = 34 / volts - 5;
            volts = ((float) result [3])* 3.3 / 4096 * 3.2;
            robotState.distanceTelemetreExGauche = 34 / volts - 5;
            volts = ((float) result [0])* 3.3 / 4096 * 3.2;
            robotState.distanceTelemetreExDroit = 34 / volts - 5;


            if (robotState.distanceTelemetreExDroit < 15) {
                LED_ORANGE = 1;
            } else {
                LED_ORANGE = 0;
            }
            if (robotState.distanceTelemetreCentre < 30) {
                LED_BLEUE = 1;
            } else {
                LED_BLEUE = 0;
            }
            if (robotState.distanceTelemetreExGauche < 15) {
                LED_BLANCHE = 1;
            } else {
                LED_BLANCHE = 0;
            }

        }
    }

} // fin main