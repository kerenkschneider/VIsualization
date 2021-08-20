import re
import pandas as pd
from pip._vendor import requests
from PIL import Image
import numpy as np

# Parse data from input cvs to three new kinds of files


def parse():
    # separate words that start with capital letters and connected to the previous word

    for i, desc in enumerate(input_data[:, 2]):
        desc = re.sub(r'(?<=[a-z])(?=[A-Z])', ' ', desc)
        desc = desc.replace(u'\xa0', ' ')
        input_data[:, 2][i] = re.sub(' +', ' ', desc)

    # save only the description and the titles in a cvs files
    pd.DataFrame(input_data[:, 2]).to_csv("desc.csv", index=False)
    pd.DataFrame(input_data[:, 4]).to_csv("title.csv", index=False)
    pd.DataFrame(input_data[:, 0]).to_csv("brand.csv", index=False)
    pd.DataFrame(input_data[:, 1]).to_csv("color.csv", index=False)

    # save all information after parsing
    pd.DataFrame(input_data).to_csv("all.csv", index=False)


def convert_jpg_to_png():
    for i in range(1, 1000):
        image_in = 'images/{}.jpeg'.format(i)
        image_out = 'pngImages/{}.png'.format(i)

        im1 = Image.open(image_in)
        im1.save(image_out)


def download_images():

    for i in range(1, len(input_data)):
        r = requests.get(input_data[:, 3][i], stream=True)
        # converts response headers mime type to an extension (may not work with everything)
        ext = r.headers['content-type'].split('/')[-1]
        # open the file to write as binary - replace 'wb' with 'w' for text files
        with open("images/%s.%s" % (i, ext), 'wb') as f:
            for chunk in r.iter_content(1024):  # iterate on stream using 1KB packets
                f.write(chunk)  # write the file


if __name__ == '__main__':
    # read cvs file
    input_data = pd.read_csv("data/dresses/dataset_dresses_3.csv").to_numpy()

    # parse the input
    parse()

    # save images
    download_images()
    convert_jpg_to_png()

    # Normalized price
    price = pd.read_csv("out/price_out.csv").to_numpy()
    price = (price - min(price)) / (max(price) - min(price))
    pd.DataFrame(price).to_csv("out/price_norm_out.csv", index=False)

