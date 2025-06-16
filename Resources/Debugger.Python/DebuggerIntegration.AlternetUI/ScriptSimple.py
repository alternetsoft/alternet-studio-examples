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

def add_numbers(a, b):
    return a + b

# Example usage
# result = add_numbers(5, 3, name=4)
# print(result)  # Outputs: 8	
	
#result = System.Math.Min(5, val2 = 5, fake = 10)	
#System.Diagnostics.Debug.WriteLine(f"Min: {result}");
