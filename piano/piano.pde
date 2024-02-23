import processing.sound.*;
SinOsc sine;

float d;
float dd;
float re;
float rere;
float mi;
float fa;
float fafa;
float so;
float soso;
float ra = 440.0;
float rara;
float si;
float r;

int octave;

void setup(){
  size(1000,500);
  sine = new SinOsc(this);
  // oto no onntei wo sakusei
  r = pow(2.0,1.0/12.0);
  text(r,900,350);
  rara = ra*r;
  si = rara*r;
  soso = ra/r;
  so = soso/r;
  fafa = so/r;
  fa = fafa/r;
  mi = fa/r;
  rere = mi/r;
  re = rere/r;
  dd = re/r;
  d = dd/r;
  
  octave = 3;
}

void octaveup(){
  if(octave<6){
  d = d*2.0;
  dd = dd*2.0;
  re = re*2.0;
  rere = rere*2.0;
  mi = mi*2.0;
  fa = fa*2.0;
  fafa = fafa*2.0;
  so = so*2.0;
  soso = soso*2.0;
  ra = ra*2.0;
  rara = rara*2.0;
  si = si*2.0;
  octave = octave+1;
}
}

void octavedown(){
  if(octave>1){
  d = d/2.0;
  dd = dd/2.0;
  re = re/2.0;
  rere = rere/2.0;
  mi = mi/2.0;
  fa = fa/2.0;
  fafa = fafa/2.0;
  so = so/2.0;
  soso = soso/2.0;
  ra = ra/2.0;
  rara = rara/2.0;
  si = si/2.0;
  octave = octave-1;
}
}

void draw(){
  background(#A4E6F2);
  fill(255);
  rectMode(CENTER);
  rect(width/2,height/2,700,350);
  stroke(0);
  rect(width/2,height/2,700,350);
  // kokken ha y zahyo wo 75 ni kuttukeru
  fill(0);
  rectMode(CORNER);
  rect(225,75,50,200);
  rect(325,75,50,200);
  rect(525,75,50,200);
  rect(625,75,50,200);
  rect(725,75,50,200);
  stroke(0);
  rect(225,75,50,200);
  rect(325,75,50,200);
  rect(525,75,50,200);
  rect(625,75,50,200);
  rect(725,75,50,200);
  
  // mouse no iti ni oujite irowo kaeru 
  int X = mouseX;
  int Y = mouseY;
  noStroke();
  fill(100);
  rectMode(CORNER);
  if(((150<=X)&&(X<250)&&(275<=Y)&&(Y<=425))||((150<=X)&&(X<225)&&(75<=Y)&&(Y<275))){
    rect(150,75,75,350);
    rect(150,275,100,150);
  }
  // do#
  if((225<=X)&&(X<275)&&(75<=Y)&&(Y<275)){
    rect(225,75,50,200);
  }
  // re
  if(((250<=X)&&(X<350)&&(275<=Y)&&(Y<=425))||((275<=X)&&(X<325)&&(75<=Y)&&(Y<275))){
    rect(275,75,50,350);
    rect(250,275,100,150);
  }
  // re#
  if((325<=X)&&(X<375)&&(75<=Y)&&(Y<275)){
    rect(325,75,50,200);
  }
  // mi
  if(((350<=X)&&(X<450)&&(275<=Y)&&(Y<=425))||((375<=X)&&(X<450)&&(75<=Y)&&(Y<275))){
    rect(375,75,75,350);
    rect(350,275,100,150);
  }
  // fa
  if(((450<=X)&&(X<550)&&(275<=Y)&&(Y<=425))||((450<=X)&&(X<525)&&(75<=Y)&&(Y<275))){
    rect(450,75,75,350);
    rect(450,275,100,150);
  }
  // fa#
  if((525<=X)&&(X<575)&&(75<=Y)&&(Y<275)){
    rect(525,75,50,200);
  }
  // so
  if(((550<=X)&&(X<650)&&(275<=Y)&&(Y<=425))||((575<=X)&&(X<625)&&(75<=Y)&&(Y<275))){
    rect(575,75,50,350);
    rect(550,275,100,150);
  }
  // so#
  if((625<=X)&&(X<675)&&(75<=Y)&&(Y<275)){
    rect(625,75,50,200);
  }
  // ra
  if(((650<=X)&&(X<750)&&(275<=Y)&&(Y<=425))||((675<=X)&&(X<725)&&(75<=Y)&&(Y<275))){
    rect(675,75,50,350);
    rect(650,275,100,150);
  }
  // ra#
  if((725<=X)&&(X<775)&&(75<=Y)&&(Y<275)){
    rect(725,75,50,200);
  }
  // si
  if(((750<=X)&&(X<850)&&(275<=Y)&&(Y<=425))||((775<=X)&&(X<850)&&(75<=Y)&&(Y<275))){
    rect(775,75,75,350);
    rect(750,275,100,150);
  }
  
  // sen wo hiku
  stroke(0);
  line(250,275,250,425);
  line(350,275,350,425);
  line(450,75,450,425);
  line(550,275,550,425);
  line(650,275,650,425);
  line(750,275,750,425);
  // zukei owari
  fill(255,0,255);
  textSize(32);
  text("Pitch level = " +octave,380,50);
  textSize(24);
  text("UP Pitch: push A key",380,450);
  text("DOWN Pitch: push S key",365,475);
}

void mousePressed(){
  float volume = 0.1;
  float X = mouseX;
  float Y = mouseY;
  // do
  if(((150<=X)&&(X<250)&&(275<=Y)&&(Y<=425))||((150<=X)&&(X<225)&&(75<=Y)&&(Y<275))){
    volume = 0.1 + (Y-75)/500;
    sine.play(d,volume);
    fill(100);
    rect(150,75,100,350);
  }
  // do#
  if((225<=X)&&(X<275)&&(75<=Y)&&(Y<275)){
    volume = 0.1 + (Y-75)/250;
    sine.play(dd,volume);
  }
  // re
  if(((250<=X)&&(X<350)&&(275<=Y)&&(Y<=425))||((275<=X)&&(X<325)&&(75<=Y)&&(Y<275))){
    volume = 0.1 + (Y-75)/500;
    sine.play(re,volume);
  }
  // re#
  if((325<=X)&&(X<375)&&(75<=Y)&&(Y<275)){
    volume = 0.1 + (Y-75)/250;
    sine.play(rere,volume);
  }
  // mi
  if(((350<=X)&&(X<450)&&(275<=Y)&&(Y<=425))||((375<=X)&&(X<450)&&(75<=Y)&&(Y<275))){
    volume = 0.1 + (Y-75)/500;
    sine.play(mi,volume);
  }
  // fa
  if(((450<=X)&&(X<550)&&(275<=Y)&&(Y<=425))||((450<=X)&&(X<525)&&(75<=Y)&&(Y<275))){
    volume = 0.1 + (Y-75)/500;
    sine.play(fa,volume);
  }
  // fa#
  if((525<=X)&&(X<575)&&(75<=Y)&&(Y<275)){
    volume = 0.1 + (Y-75)/250;
    sine.play(fafa,volume);
  }
  // so
  if(((550<=X)&&(X<650)&&(275<=Y)&&(Y<=425))||((575<=X)&&(X<625)&&(75<=Y)&&(Y<275))){
    volume = 0.1 + (Y-75)/500;
    sine.play(so,volume);
  }
  // so#
  if((625<=X)&&(X<675)&&(75<=Y)&&(Y<275)){
    volume = 0.1 + (Y-75)/250;
    sine.play(soso,volume);
  }
  // ra
  if(((650<=X)&&(X<750)&&(275<=Y)&&(Y<=425))||((675<=X)&&(X<725)&&(75<=Y)&&(Y<275))){
    volume = 0.1 + (Y-75)/500;
    sine.play(ra,volume);
  }
  // ra#
  if((725<=X)&&(X<775)&&(75<=Y)&&(Y<275)){
    volume = 0.1 + (Y-75)/250;
    sine.play(rara,volume);
  }
  // si
  if(((750<=X)&&(X<850)&&(275<=Y)&&(Y<=425))||((775<=X)&&(X<850)&&(75<=Y)&&(Y<275))){
    volume = 0.1 + (Y-75)/500;
    sine.play(si,volume);
  }
}

void mouseReleased() {
 sine.stop();
}

void keyPressed(){
  if(key == 'a')octaveup();
  if(key == 's')octavedown();
}
