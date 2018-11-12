import pandas as pd
import os
import json


pd.options.display.max_columns = 20


def encode(string: str):
    """
    Encoded link string for most "security"
    chr - from ASCII code on char
    ord - from char on ASCII code

    :param string:
    :return:
    """
    list_encode = []

    for char in string:
        list_encode.append(ord(char))

    a = []
    for i in range(len(list_encode)):
        a.append(chr(list_encode[i]+3))
    return ''.join(a)


def decode(string: str):
    """
    Decoded encoded link
    chr - from ASCII code on char
    ord - from char on ASCII code

    :param string:
    :return:
    """
    list_encode = []

    for char in string:
        list_encode.append(chr(ord(char)-3))
    return ''.join(list_encode)


def gen_links(xls_file: str):
    """
    Reading xls file, parsing and return link formed from the IIN and login

    :param xls_file:
    :return:
    """
    file_g = os.path.join(os.getcwd(), xls_file)
    xls = pd.ExcelFile(file_g)
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


def post_and_name(xls_file: str):
    """
    Returning email and name

    :param xls_file:
    :return:
    """
    file_p = os.path.join(os.getcwd(), xls_file)
    xls = pd.ExcelFile(file_p)
    df = xls.parse(xls.sheet_names[0])
    name = df.keys()[2]
    email = df.keys()[3]
    name_list = [x if not isinstance(x, float) else '' for x in df[name]]
    email_list = [x if not isinstance(x, float) else '' for x in df[email]]

    return name_list, email_list


if __name__ == '__main__':
    list_end = []

    for x in gen_links('dannye_s_its.xlsx'):
        list_end.append(encode(x))

    file = open('links.json', 'w')
    list_data = []
    for i in range(len(list_end)):
        list_data.append({
            "name": post_and_name('dannye_s_its.xlsx')[0][i],
            "email": post_and_name('dannye_s_its.xlsx')[1][i],
            "link_raw": gen_links('dannye_s_its.xlsx')[i],
            "linnk_encode": list_end[i]
        })

    json_list = json.dumps(list_data, sort_keys=True, indent=4, ensure_ascii=False)
    file.write(json_list)
