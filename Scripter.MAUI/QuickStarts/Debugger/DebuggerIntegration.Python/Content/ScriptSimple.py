import time

## f = open("demofile2.txt", "a")
## f.write("Now the file has more content!")
## f.close()

print("Hello, world!")

## print ('argument list', sys.argv)

## input() works fine in case of running without debug
name = "my friend"

print("Hello, " + name + "!");

## try: x
## except NameError: x = 5
## else: x = x + 1

## print("x = " + str(x));

for i in range(100):
    print(f"Cycle {i+1} [This is sample text]")
    ## time.sleep(0.1)
    
import importlib.metadata

packages = importlib.metadata.distributions()
for pkg in packages:
    print(pkg.metadata["Name"])


random = System.Random();
randomValue = random.Next(1, 100); 
print("randomValue = " + str(randomValue));
