# AppStore assets

This repository contains all the assets used for AppStores.

- [OpenTTD's Steam Store Page](https://store.steampowered.com/app/1536610/OpenTTD/).

## Folder structure

In `assets` are all the assets specific to a single store.
Most often, those have very specific (size) requirements and are as such custom-made for that store.

In `lang` are the translations of blobs of text used on the stores.
As most stores use the same blobs, those are as generic as possible, and do not mention the stores itself.
[eints](https://translator.openttd.org) takes care of the translations.

In `news` are the assets used for news posts on various of stores.

In `screenshots` is our collection of screenshots we publish on stores.

## Language mapping for Steam

[eints](https://translator.openttd.org) identifies languages different than Steam. This is the mapping we use:

| Steam Name            | Steam API language code | Steam Web API language code | Eints ISO code | Language file name       |
| --------------------- | ----------------------- | --------------------------- | -------------- | ------------------------ |
| Arabic                | arabic                  | ar                          | ar_EG          | arabic_egypt.txt         |
| Bulgarian             | bulgarian               | bg                          | bg_BG          | bulgarian.txt            |
| Chinese (Simplified)  | schinese                | zh-CN                       | zh_CN          | simplified_chinese.txt   |
| Chinese (Traditional) | tchinese                | zh-TW                       | zh_TW          | traditional_chinese.txt  |
| Czech                 | czech                   | cs                          | cs_CZ          | czech.txt                |
| Danish                | danish                  | da                          | da_DK          | danish.txt               |
| Dutch                 | dutch                   | nl                          | nl_NL          | dutch.txt                |
| English               | english                 | en                          | en_GB          | english.txt              |
| Finnish               | finnish                 | fi                          | fi_FI          | finnish.txt              |
| French                | french                  | fr                          | fr_FR          | french.txt               |
| German                | german                  | de                          | de_DE          | german.txt               |
| Greek                 | greek                   | el                          | el_GR          | greek.txt                |
| Hungarian             | hungarian               | hu                          | hu_HU          | hungarian.txt            |
| Italian               | italian                 | it                          | it_IT          | italian.txt              |
| Japanese              | japanese                | ja                          | ja_JP          | japanese.txt             |
| Korean                | koreana                 | ko                          | ko_KR          | korean.txt               |
| Norwegian             | norwegian               | no                          | nb_NO          | norwegian_bokmal.txt     |
| Polish                | polish                  | pl                          | pl_PL          | polish.txt               |
| Portuguese            | portuguese              | pt                          | pt_PT          | portuguese.txt           |
| Portuguese-Brazil     | brazilian               | pt-BR                       | pt_BR          | brazilian_portuguese.txt |
| Romanian              | romanian                | ro                          | ro_RO          | romanian.txt             |
| Russian               | russian                 | ru                          | ru_RU          | russian.txt              |
| Spanish-Spain         | spanish                 | es                          | es_ES          | spanish.txt              |
| Spanish-Latin America | latam                   | es-419                      | es_MX          | spanish_MX.txt           |
| Swedish               | swedish                 | sv                          | sv_SE          | swedish.txt              |
| Thai                  | thai                    | th                          | th_TH          | thai.txt                 |
| Turkish               | turkish                 | tr                          | tr_TR          | turkish.txt              |
| Ukrainian             | ukrainian               | uk                          | uk_UA          | ukrainian.txt            |
| Vietnamese            | vietnamese              | vn                          | vi_VN          | vietnamese.txt           |

## Git LFS

We use Git LFS to store the source of assets, as they can be pretty big.
This means that for example the `.png` files are NOT in LFS, but the `.xcf` are.
People checking out the GitHub repository without having LFS installed will see all assets how they are, but are unable to edit them.
You ned to install Git LFS to get the source of assets for editing.
See [here](https://docs.github.com/en/github/managing-large-files/installing-git-large-file-storage) for a manual on how to do just that.
