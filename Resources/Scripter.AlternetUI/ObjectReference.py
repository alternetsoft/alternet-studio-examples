def ChangeButtonLocation(o, e):
  autoRand = System.Random()
  x = (int)(35 * autoRand.Next(0, 10) + 1)
  y = (int)(15 * autoRand.Next(0, 10) + 1)
  RunButton.Margin = Alternet.UI.Thickness(x, y, 0, 0)

def RunButtonClick(o,e):
  timer.Stop()
  RunButton.Text = "Test Button"

def Main(buttonText):
  RunButton.Click += RunButtonClick
  RunButton.Text = buttonText
  timer.Interval = 1000
  timer.Tick += ChangeButtonLocation
  timer.Start()
