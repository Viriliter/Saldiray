import struct
from decimal import *
import base64
#print map(hex,struct.unpack('B',struct.pack('B',0)))
def int_to_bytes(value, length):
    result = []

    for i in range(0, length):
        result.append(value >> (i * 8) & 0xff)

    result.reverse()

    return result
def bytes_to_int(bytes):
    result = 0

    for b in bytes:
        result = result * 256 + int(b)

    return result
print (str(127).encode())