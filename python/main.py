import pandas as pd
import os
import rsa
pd.options.display.max_columns = 20


def decode(string: str):
    (bob_pub, bob_priv) = rsa.newkeys(512)
    message = string.encode('utf-8')
    crypto = rsa.encrypt(message, bob_pub)
    return (crypto, bob_priv)


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


if __name__ == '__main__':
    list_end = []
    for x in gen_links('dannye_s_its.xlsx'):
        list_end.append(decode(x))

    file = open('link.txt', 'w')
    for x in list_end:
        file.write(f'{x[0]}\n')
    file.write(str(list_end[0][1]))