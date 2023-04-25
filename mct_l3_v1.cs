IOPult ioPult = IOPult.GetCurrentPult();

// Vorbereitungaufgabe 1: Bitweise Operatoren
enum Schalter
{
	S0 = 0b_0000_0000,
	S1 = 0b_0000_0001,
	S2 = 0b_0000_0010,
	S3 = 0b_0000_0011,
	S4 = 0b_0000_0100,
	S5 = 0b_0000_0101,
	S6 = 0b_0000_0110,
	S7 = 0b_0000_0111
}

enum LED
{
	D0 = 0b_0000_0000,
	D1 = 0b_0000_0001,
	D2 = 0b_0000_0010,
	D3 = 0b_0000_0011,
	D4 = 0b_0000_0100,
	D5 = 0b_0000_0101,
	D6 = 0b_0000_0110,
	D7 = 0b_0000_0111
}

byte portB = ioPult.PortB;

// a,b)
if (portB && Schalter.S5)
{

}

// c)
 ioPult.PortA = ioPult.PortA | LED.D0;