
import MyModule
from datetime import date

class MyClass:
    """A simple example class"""
    i = 12345

    def __init__(self):
        self.my_instance_var = 34
    
    def f(self):
        xx = 'hello world'
        return xx


def ChangeMenuItem():
    TestMenuItem.Header += "1"
    TestMenuItem.IsEnabled = not TestMenuItem.IsEnabled

dispatcher = TestMenuItem.Dispatcher
dispatcher.Invoke(Action(ChangeMenuItem))

x = 123
a = [1, 2, 3, 4, 5]
e = date.today()
c = MyClass()

hw = c.f()

#while True:
#    print(x)

MessageBox.Show("From Module: " + MyModule.GetString())

def MyFunction(message):
    MessageBox.Show("MyFunction: " + message)
