import System.Windows.Forms
#This code is reloads changes from designed form.
import Form1_Designer
from importlib import reload
reload(Form1_Designer)
from Form1_Designer import *


class Form1(Form1_Designer):
    
    def __init__(self):
        super().__init__()
        self.InitializeComponent()
    
    @staticmethod
    def RunForm():
        form = Form1()
        form.ShowDialog()
        form.Dispose()

Form1.RunForm()
