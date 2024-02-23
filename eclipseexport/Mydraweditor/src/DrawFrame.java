import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.util.*;
import javax.swing.border.*;
 
// 描画した図形を記録する Figure クラス (継承して利用する)
class Figure {
  protected int x, y, width, height;
  protected Color color;
  public Figure(int x, int y, int w, int h, Color c) {
    this.x = x; this.y = y;  // this.x, this.y はインスタンス変数．
    width = w; height = h;   // ローカル変数で同名の変数がある場合は，this
    color = c;               // を付けると，インスタンス変数を指す．
  }
  public void setSize(int w, int h) {
    width = w; height = h;
  }
  public void setLocation(int x, int y) {
    this.x = x; this.y = y;
  }
  public void reshape(int x1, int y1, int x2, int y2, int cs) {
    int newx = Math.min(x1, x2);
    int newy = Math.min(y1, y2);
    int neww = Math.abs(x1 - x2);
    int newh = Math.abs(y1 - y2);
    if(cs==3) {
    	newx = x1;
    	newy = y1;
    	neww = x2;
    	newh = y2;
    }
    setLocation(newx, newy);
    setSize(neww, newh);
  }
  public void draw(Graphics g) {}
}
 
class RectangleFigure extends Figure {
  public RectangleFigure(int x, int y, int w, int h, Color c) {
    super(x, y, w, h, c);
    // 引数付きのコンストラクタは継承されないので，コンストラクタを定義．
    // superで親のコンストラクタを呼び出すだけ．
  }
  public void draw(Graphics g) {
    g.setColor(color);
    g.drawRect(x, y, width, height);
  }
}
class FillrectFigure extends Figure {
	  public FillrectFigure(int x, int y, int w, int h, Color c) {
		    super(x, y, w, h, c);
		    // 引数付きのコンストラクタは継承されないので，コンストラクタを定義．
		    // superで親のコンストラクタを呼び出すだけ．
		  }
		  public void draw(Graphics g) {
		    g.setColor(color);
		    g.fillRect(x, y, width, height);
		  }
		}
class OvalFigure extends Figure {
	  public OvalFigure(int x, int y, int w, int h, Color c) {
		    super(x, y, w, h, c);
		    // 引数付きのコンストラクタは継承されないので，コンストラクタを定義．
		    // superで親のコンストラクタを呼び出すだけ．
		  }
		  public void draw(Graphics g) {
		    g.setColor(color);
		    g.drawOval(x, y, width, height);
	}
}
class FillovalFigure extends Figure {
	  public FillovalFigure(int x, int y, int w, int h, Color c) {
		    super(x, y, w, h, c);
		    // 引数付きのコンストラクタは継承されないので，コンストラクタを定義．
		    // superで親のコンストラクタを呼び出すだけ．
		  }
		  public void draw(Graphics g) {
		    g.setColor(color);
		    g.fillOval(x, y, width, height);
	}
}
class LineFigure extends Figure {
	  public LineFigure(int x, int y, int w, int h, Color c) {
		  super(x, y, w, h, c);
		    // 引数付きのコンストラクタは継承されないので，コンストラクタを定義．
		    // superで親のコンストラクタを呼び出すだけ．
		  }
		  public void draw(Graphics g) {
		    g.setColor(color);
		    g.drawLine(x,y,width,height);
	}
}
////////////////////////////////////////////////
// Model (M)
 
// modelは java.util.Observableを継承する．Viewに監視される．
class DrawModel extends Observable {
  protected ArrayList<Figure> fig;
  protected Figure drawingFigure,f;
  protected Color currentColor;
  int ff;//1なら四角形、2なら円、3なら直線
  public DrawModel() {
    fig = new ArrayList<Figure>();
    drawingFigure = null;
    currentColor = Color.red;
    ff = 1;
  }
  public ArrayList<Figure> getFigures() {
    return fig;
  }
  public Figure getFigure(int idx) {
    return fig.get(idx);
  }
  public void createFigure(int x, int y) {
	if(ff==1) {
		f = new RectangleFigure(x, y, 0, 0, currentColor);
	}
	else if(ff==2) {
    	f = new OvalFigure(x,y,0,0,currentColor);
    }
	else if(ff==3) {
    	f = new LineFigure(x,y,x,y,currentColor);
    }
	else if(ff==4) {
		f = new FillrectFigure(x,y,0,0,currentColor);
	}
	else if(ff==5) {
		f = new FillovalFigure(x,y,0,0,currentColor);
	}
    fig.add(f);
    drawingFigure = f;
    setChanged();
    notifyObservers();
  }
  public void reshapeFigure(int x1, int y1, int x2, int y2) {
    if (drawingFigure != null) {
      drawingFigure.reshape(x1, y1, x2, y2,ff);
      setChanged();
      notifyObservers();
    }
  }
}
 
////////////////////////////////////////////////
// View (V)
 
// Viewは，Observerをimplementsする．Modelを監視して，
// モデルが更新されたupdateする．実際には，Modelから
// update が呼び出される．
class ViewPanel extends JPanel implements Observer {
  protected DrawModel model;
  public ViewPanel(DrawModel m, DrawController c) {
    this.setBackground(Color.white);
    this.addMouseListener(c);
    this.addMouseMotionListener(c);
    model = m;
    model.addObserver(this);
  }
  public void paintComponent(Graphics g) {
    super.paintComponent(g);
    ArrayList<Figure> fig = model.getFigures();
    for(int i = 0; i < fig.size(); i++) {
      Figure f = fig.get(i);
      f.draw(g);
    }
  }
  public void update(Observable o, Object arg) {
    repaint();
  }
}
class ButtonPanelcolor extends JPanel implements ActionListener{
	private JButton b1,b2,b3,b4,b5,b6;
	protected DrawModel model;
	public ButtonPanelcolor(DrawModel a){
	  model = a;
	  b1 = new JButton("red");
	  b2 = new JButton("green");
	  b3 = new JButton("blue");
	  b4 = new JButton("black");
	  b5 = new JButton("yellow");
	  b6 = new JButton("reset");
	  b1.addActionListener(this);
	  b2.addActionListener(this);
	  b3.addActionListener(this);
	  b4.addActionListener(this);
	  b5.addActionListener(this);
	  b6.addActionListener(this);
	  setLayout(new GridLayout(1,6));
	  add(b1);add(b2);add(b3);add(b4);add(b5);add(b6);
	}
	public void actionPerformed(ActionEvent e) {
		if(e.getSource() == b1) {
			model.currentColor = Color.red ;
		}
		else if(e.getSource() == b2) {
			model.currentColor=Color.green;
		}
		else if(e.getSource() == b3) {
			model.currentColor=Color.blue;
		}
		else if(e.getSource() == b4) {
			model.currentColor=Color.black;
		}
		else if(e.getSource() == b5) {
			model.currentColor=Color.yellow;
		}
		else if(e.getSource() == b6) {
			model.fig = new ArrayList<Figure>();
			model.createFigure(0,0);
			model.fig = new ArrayList<Figure>();
			model.drawingFigure = null;
		}
	}
}
class ButtonPanelfigure extends JPanel implements ActionListener{
	private JButton b1,b2,b3,b4,b5;
	protected DrawModel model;
	public ButtonPanelfigure(DrawModel a){
	  model = a;
	  b1 = new JButton("Rectangle");
	  b2 = new JButton("Oval");
	  b3 = new JButton("Line");
	  b4 = new JButton("FillRect");
	  b5 = new JButton("FillOval");
	  b1.addActionListener(this);
	  b2.addActionListener(this);
	  b3.addActionListener(this);
	  b4.addActionListener(this);
	  b5.addActionListener(this);
	  setLayout(new GridLayout(5,1));
	  add(b1);add(b4);add(b2);add(b5);add(b3);
	}
	public void actionPerformed(ActionEvent e) {
		if(e.getSource() == b1) {
			model.ff = 1;
		}
		else if(e.getSource() == b2) {
			model.ff = 2;
		}
		else if(e.getSource() == b3) {
			model.ff = 3;
		}
		else if(e.getSource() == b4) {
			model.ff = 4;
		}
		else if(e.getSource() == b5) {
			model.ff = 5;
		}
		/*else if(e.getSource() == b6) {
			new DrawFrame();
		}*/
	}
}
//////////////////////////////////////////////////
// Main class
//   (GUIを組み立てているので，view の一部と考えてもよい)
class DrawFrame extends JFrame {
  DrawModel model;
  ViewPanel view;
  ButtonPanelcolor p1;
  ButtonPanelfigure p2;
  DrawController cont;
  public DrawFrame() {
    model = new DrawModel();
    p1 = new ButtonPanelcolor(model);
    p2 = new ButtonPanelfigure(model);
    cont = new DrawController(model);
    view = new ViewPanel(model,cont);
    this.setBackground(Color.black);
    this.setTitle("Draw Editor");
    this.setSize(500, 500);
    view.setBorder(new LineBorder(Color.blue,3));
    //setLayout(new GridLayout(2,1));
    this.add(p1,BorderLayout.NORTH);
    this.add(p2,BorderLayout.WEST);
    this.add(view);
    this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    this.setVisible(true);
  }
  public static void main(String[] args) {
    new DrawFrame();
  }
}
 
////////////////////////////////////////////////
// Controller (C)
 
class DrawController implements MouseListener, MouseMotionListener {
  protected DrawModel model;
  protected int dragStartX, dragStartY;
  public DrawController(DrawModel a) {
    model = a;
  }
  public void mouseClicked(MouseEvent e) {}
  public void mousePressed(MouseEvent e) {
    dragStartX = e.getX(); dragStartY = e.getY();
    model.createFigure(dragStartX, dragStartY);
  }
  public void mouseDragged(MouseEvent e) {
    model.reshapeFigure(dragStartX, dragStartY, e.getX(), e.getY());
  }
  public void mouseReleased(MouseEvent e) {}
  public void mouseEntered(MouseEvent e) {}
  public void mouseExited(MouseEvent e) {}
  public void mouseMoved(MouseEvent e) {}
}
