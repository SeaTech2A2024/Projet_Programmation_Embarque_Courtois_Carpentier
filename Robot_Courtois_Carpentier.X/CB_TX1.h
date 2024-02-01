#ifndef CB_TX1_H
#define CB_TX1_H

#include <xc.h>
#include <stdio.h>
#include <stdlib.h>

#define CBTX1_BUFFER_SIZE 128

extern int cbTx1Head;
extern int cbTx1Tail;
extern unsigned char cbTx1Buffer[CBTX1_BUFFER_SIZE];
extern unsigned char isTransmitting;

void SendMessage(unsigned char* message, int length);
void CB_TX1_Add(unsigned char value);
unsigned char CB_TX1_Get(void);
void __attribute__((interrupt, no_auto_psv)) _U1TXInterrupt(void);
void SendOne(void);
unsigned char CB_TX1_IsTransmitting(void);
int CB_TX1_GetDataSize(void);
int CB_TX1_RemainingSize(void);
#endif /* CB_TX1_H */
