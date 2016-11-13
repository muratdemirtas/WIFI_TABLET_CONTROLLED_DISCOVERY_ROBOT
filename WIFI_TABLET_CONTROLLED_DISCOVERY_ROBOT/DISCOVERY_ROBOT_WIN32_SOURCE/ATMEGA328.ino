float sicaklik; //Analog değeri dönüştüreceğimiz sıcaklık 
float analoggerilim,analoggerilim2; //Ölçeceğimiz analog değer
String inputString = "";         // a string to hold incoming data
boolean stringComplete = false;  // whether the string is complete

#define echoPin 4 // Echo Pin
#define trigPin 3 // Trigger Pin
String veri;
#define echoPin2 6 // Echo Pin
#define trigPin2 5 // Trigger Pin
int maximumRange = 200; // Maximum range needed
int minimumRange = 0; // Minimum range needed
long duration,duration2, distance,distance2; // Duration used to calculate distance
int gaz_degeri,batarya=0;
float vout;
void setup () {
pinMode(trigPin, OUTPUT);
pinMode(8, OUTPUT);
pinMode(7, OUTPUT);
 pinMode(echoPin, INPUT);
 pinMode(trigPin2, OUTPUT);
 pinMode(echoPin2, INPUT);
  inputString.reserve(200);
 Serial.begin(115200); //Seri haberleşme,Sıcaklığı ekranda görücez
}


void onmesafe() {
 digitalWrite(trigPin2, LOW); 
 delayMicroseconds(2); 
 digitalWrite(trigPin2, HIGH);
 delayMicroseconds(10); 
 digitalWrite(trigPin2, LOW);
 duration2 = pulseIn(echoPin2, HIGH);
 distance2 = duration2/58.2;
 delay(50);
  }
 
void arkamesafe() {
 digitalWrite(trigPin, LOW); 
 delayMicroseconds(2); 
 digitalWrite(trigPin, HIGH);
 delayMicroseconds(10); 
 digitalWrite(trigPin, LOW);
 duration = pulseIn(echoPin, HIGH);
 distance = duration/58.2;
 delay(50);
}

void sicaklikolc() {
 analoggerilim = analogRead(A0); //A1'den değeri ölç
 analoggerilim = (analoggerilim/1023)*5000;//değeri mV'a dönüştr 
 sicaklik = analoggerilim /10,0; // mV'u sicakliğa dönüştür

 delay(50);
 }
 
void gaz() {
 gaz_degeri = analogRead(A5); //A1'den değeri ölç
 delay(50);
  }
 
 void bataryaolc() {
analoggerilim2  = analogRead(A3); //A1'den değeri ölç
analoggerilim2 = (analoggerilim2/1023)*5000;//değeri mV'a dönüştr 
vout = (analoggerilim2 * 5.0) / 1024.0; 
   }

void loop () {
 
    
  if (stringComplete) {
    if(String(inputString)=="BAh") 
    digitalWrite(8,HIGH);
  
    
    else if (String(inputString)=="BAo")
     digitalWrite(8,LOW);
     
     if(String(inputString)=="BAb\n")
    digitalWrite(7,HIGH);
    else if (String(inputString)=="BAk\n")
     digitalWrite(7,LOW);
   
    // clear the string:
    inputString = "";
    stringComplete = false;
  }
  
  
gaz();
onmesafe();
arkamesafe();
bataryaolc();
sicaklikolc();
delay(300);

veri += "-";
veri += gaz_degeri;
veri += "-";
veri += distance2;
veri += "-";
veri += distance;
veri += "-";
veri += vout*2;
veri += "-";
veri += sicaklik;
Serial.println(veri);
veri="";
Serial.flush();
}

void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read();
    // add it to the inputString:
    inputString += inChar;
    // if the incoming character is a newline, set a flag
    // so the main loop can do something about it:
    if (inChar == '\n') {
      stringComplete = true;
    }
  }
}

