import threading
import time
import sys

def func():
    i = 1
    while True:
        time.sleep(0.5)
        print(f"({i}) Thread alive, and it won't die on program termination")
        i += 1  # Increment `i` on each loop        

t1 = threading.Thread(target=func)
t1.start()
time.sleep(200)

