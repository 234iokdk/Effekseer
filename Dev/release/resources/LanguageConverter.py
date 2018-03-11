import pandas as pd
import json
import collections as cl
import codecs

books = ['Languages.xlsx']

values_en = cl.OrderedDict()
values_ja = cl.OrderedDict()
kv_en = {}
kv_ja = {}

for book in books:
    file = pd.ExcelFile(book)
    for sheet in file.sheet_names:
        parsed = file.parse(sheet)
        for index, row in parsed.iterrows():
            kv_en[str(row[0])] = str(row[1])
            kv_ja[str(row[0])] = str(row[2])
            
values_en['kv'] = kv_en
values_ja['kv'] = kv_ja

f_en = codecs.open('lang_en.json','w', "utf-8")
json.dump(values_en,f_en, ensure_ascii=False, indent=2)

f_ja = codecs.open('lang_ja.json','w', "utf-8")
json.dump(values_ja,f_ja, ensure_ascii=False, indent=2)
f_ja.close()