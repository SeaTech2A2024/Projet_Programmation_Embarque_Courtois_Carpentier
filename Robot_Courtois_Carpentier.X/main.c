#include <stdio.h>
#include <stdlib.h>
#include <xc.h>
#include "ChipConfig.h"
#include "IO.h"
#include "timer.h"
#include "PWM.h"

int main(void)
{
    /***************************************************************************************************/
    //Initialisation de l?oscillateur
    /****************************************************************************************************/
    InitOscillator();
    /****************************************************************************************************/
    // Configuration des éentres sorties
    /****************************************************************************************************/
    InitIO();
    LED_BLANCHE = 1;
    LED_BLEUE = 1;
    LED_ORANGE = 1;
    
    InitTimer1();
    InitTimer23();
    
    _T3Interrupt();
    
    //InitPWM();
    //PWMSetSpeed(20, MOTEUR_DROIT);
    //PWMSetSpeed(20, MOTEUR_GAUCHE);


    /****************************************************************************************************/
    // Boucle Principale
    /****************************************************************************************************/
    while (1) {
        //LED_BLANCHE = !LED_BLANCHE;
        //LED_ORANGE = !LED_ORANGE;
        //LED_BLEUE = !LED_BLEUE;
    } // fin main
}
