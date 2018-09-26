import pandas as pd
import os
import rsa
from Crypto.PublicKey import RSA
from Crypto.Cipher import PKCS1_OAEP
from Crypto import Random
from Crypto.Cipher import AES
import base64
import hashlib


class AESCipher(object):

    def __init__(self, key):
        self.bs = 32
        self.key = hashlib.sha256(key.encode()).digest()

    def encrypt(self, raw):
        raw = self._pad(raw)
        iv = Random.new().read(AES.block_size)
        cipher = AES.new(self.key, AES.MODE_CBC, iv)
        return base64.b64encode(iv + cipher.encrypt(raw))

    def decrypt(self, enc):
        enc = base64.b64decode(enc)
        iv = enc[:AES.block_size]
        cipher = AES.new(self.key, AES.MODE_CBC, iv)
        return self._unpad(cipher.decrypt(enc[AES.block_size:])).decode('utf-8')

    def _pad(self, s):
        return s + (self.bs - len(s) % self.bs) * chr(self.bs - len(s) % self.bs)

    @staticmethod
    def _unpad(s):
        return s[:-ord(s[len(s)-1:])]


pd.options.display.max_columns = 20


def decode(string: str):
    (bob_pub, bob_priv) = rsa.newkeys(512)
    message = string.encode('utf-8')
    crypto = rsa.encrypt(message, bob_pub)
    return (crypto, bob_pub)


def gen_links(xls_file: str):
    file = os.path.join(os.getcwd(), xls_file)
    xls = pd.ExcelFile(file)
    df = xls.parse(xls.sheet_names[0])
    INN = df.keys()[0]
    login = df.keys()[1]
    INN_raw_list = [x for x in df[INN]]
    login_list = [x for x in df[login]]
    INN_list = []
    for x in INN_raw_list:
        try:
            INN_list.append([t for t in x.split() if t.isdigit()])
        except AttributeError as e:
            INN_list.append([None, None])

    link_list = []
    for x in range(58):
        if INN_list[x][0] != None:
            link_list.append(f'inn={INN_list[x][0]}&login={login_list[x]}')
        else:
            link_list.append(f'inn={INN_list[x-1][0]}&login={login_list[x]}')
    return link_list


def generate_keys():
    modulus_length = 1024

    key = RSA.generate(modulus_length)
    #print (key.exportKey())

    pub_key = key.publickey()
    # print (pub_key.exportKey())

    return key, pub_key


def encrypt_private_key(a_message, private_key):
    encryptor = PKCS1_OAEP.new(private_key)
    encrypted_msg = encryptor.encrypt(a_message)
    # print(encrypted_msg)
    encoded_encrypted_msg = base64.b64encode(encrypted_msg)
    # print(encoded_encrypted_msg)
    return encoded_encrypted_msg


def decrypt_public_key(encoded_encrypted_msg, public_key):
    encryptor = PKCS1_OAEP.new(public_key)
    decoded_encrypted_msg = base64.b64decode(encoded_encrypted_msg)
    # print(decoded_encrypted_msg)
    decoded_decrypted_msg = encryptor.decrypt(decoded_encrypted_msg)
    # print(decoded_decrypted_msg)
    return decoded_decrypted_msg


def main(message, public):
  encoded = encrypt_private_key(message.encode('utf-8'), public)
  return encoded


if __name__ == '__main__':
    list_end = []
    private, public = generate_keys()

    for x in gen_links('dannye_s_its.xlsx'):
        list_end.append(main(x, public))

    for x in list_end:
        print(decrypt_public_key(x, private))

    file = open('link.txt', 'w')

    for x in list_end:
        file.write(f'{x}\n')

    file.write('\n\n')
    file.write(private.exportKey().decode('utf-8'))
    file.write('\n\n')
    file.write(public.exportKey().decode('utf-8'))
    # straq = 'blyat'
    # aa = AESCipher('govno')
    # asd = aa.encrypt()
    # print(asd)
