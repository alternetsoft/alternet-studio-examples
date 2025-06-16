@doc_template(
    description='Loads an image from the given file path.',
    params=[
        ('image_filePath', 'str', 'The path to the image file.'),
        ('image_size', 'int', 'Size of the image.')
    ],
    recommended_params=[
        ('image_filePath="image1.png"',),
        ('image_filePath="image2.png"',),
        ('image_size=16',),
        ('image_size=32',)
    ],
    returns='image: wpf Image'
)

def LoadImage(image_filePath, image_Size):
  retMat = ImageProcess.Load(image_filePath, image_Size)
  display_image(retMat)
  return retMat


@doc_template(
    description='Loads an image from the given file path.',
    params=[
        ('image_filePath1', 'str', 'The path to the image file.')
    ],
    recommended_params=[
        ('image_filePath1="image1.png"',),
        ('image_filePath1="image2.png"',)
    ],
    returns='image: wpf Image'
)

def LoadImage1(image_filePath1):
  retMat = ImageProcess.Load(image_filePath1)
  display_image(retMat)
  return retMat

LoadImage