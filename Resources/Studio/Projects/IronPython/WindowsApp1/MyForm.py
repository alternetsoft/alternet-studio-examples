import MyForm_Designer

from MyForm_Designer import *
 
class MyForm (MyForm_Designer):  
    """A form class"""
   
    def __init__(self):
        self.InitializeComponent();
  
        
    @staticmethod
    def  RunForm():
        form = MyForm()
        form.ShowDialog()
        form.Dispose()
	
System.Console.WriteLine("Hello from Python")
MyForm.RunForm()

  
