import System.Windows.Forms
import clr

class Form1(Form1_Designer):
    
    def __init__(self):
        self.InitializeComponent()
    
    @staticmethod
    def RunForm():
        form = Form1()
        form.ShowDialog()
        form.Dispose()

Form1.RunForm()
