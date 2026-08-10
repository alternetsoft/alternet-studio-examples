# ExternalAssembly.CustomClass is declared in ExternalAssembly.dll
# which is stored as embedded resource in the application
customClass = ExternalAssembly.CustomClass()

# Call instance method
customClass.TestMethod(1, True)

# Call static method
ExternalAssembly.CustomClass.RunCommand("echo hello")
