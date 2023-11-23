#ifndef ADC_H
#define ADC_H

#include <xc.h>

extern unsigned char ADCResultIndex;
extern unsigned int ADCResult[4];
extern unsigned char ADCConversionFinishedFlag;
extern unsigned int * result;
extern unsigned int ADCValue0;
extern unsigned int ADCValue1;
extern unsigned int ADCValue2;

void InitADC1(void);
void __attribute__((interrupt, no_auto_psv)) _AD1Interrupt(void);
void ADC1StartConversionSequence(void);
unsigned int *ADCGetResult(void);
unsigned char ADCIsConversionFinished(void);
void ADCClearConversionFinishedFlag(void);

#endif  // ADC_H
