# National id validation
Basic code for validating national ID input like the nordic personal ids / birth numbers, SSN, NHS, and more.
To be used for input validation in systems that need or allows input of personal ids for use in healthcare, national / government systems and more.
If you feel like adding other countries and validation systems, feel free to do so :)

## Norwegian personal id
[no:Wikipedia - Fødselsnummer](https://no.wikipedia.org/wiki/Fødselsnummer)  
[en:Wikipedia - National identification numbers](https://en.wikipedia.org/wiki/National_identification_number)  
The Norwegian personal id validates several identification schemes, all based on the same number validation checksum.
The validation scheme is based on a weigthed Luhn algorithm with 2 control digits given  
```
    ddMMyyiiikk --> d1, d2, M1, M2, y3, y4, i1, i2, i3, k1, k2
    k1 = 11 -  ((3 x d1 + 7 x d2 + 6 x M1 + 1 x M2 + 8 x y3 + 9 x y4 + 4 x i1 + 5 x i2, +2 x i3) mod 11)
    k2 = 11 - ((5 x d1 + 4 x d2 + 3 x M1 + 2 x M2 + 7 x y3 + 6 x y4 + 5 x i1 + 4 x i2 + 3 x i3 + 2 x k1) mod 11)
```
There are two official numbering schemes that are always valid, the birth number given to all citizens, and the D-number for all immigrants and tax payers without birth number that require identification for tax and identification purposes.
In addition there are local help-number (H-numbers) and common help-numbers (FH-numbers) for use in healthcare on patients where the birth-number or D-number is not known, or where patient is a foreigner without other valid identification.

### Birth-numbers and D-numbers
Birth numbers starts with the persons birth-date in ddMMyy format, followed by the individual identification, which gives the century for the year portion following the table below, and gender based on last individual number being even for females and odd for males.
```
| Individual numbers | Years (y3, y4) | Century     |
| ------------------ | -------------- | ----------- |
| 500 - 749          | > 54           | 1855 - 1899 |
| 000 - 499          |                | 1900 - 1999 |
| 900 - 999          | > 39           | 1940 - 1999 |
| 500 - 999          | < 40           | 2000 - 2039 |
```

A D-number is recognized by having the first digit added by 4, also note that the birth date is not always certain in case of refugee immigrants.

### H-Numbers
H-Numbers follow the same structure as for birth numbers, but is recognized by having the third digit added with 4. A H-Number is not valid unless accompanied with data about the number issuer. The birth-date here may be estimated, and the use of this number is mainly for Jane or John Does that needs medical care before identification is established.

### FH-Numbers
FH-Numbers is the same as a H-Number, but is valid throughout Norwegian healthcare and registered with a central record repository. You can recognize this number by the first digit being 8 or 9. You can not get birth-date or gender based on this id, as this is a pure serial-number generation (though following the same modulo controls).

## Danish personal id
[da:Wikipedia - CPR-nummer](https://da.wikipedia.org/wiki/CPR-nummer)  
[en:Wikipedia - National identification numbers](https://en.wikipedia.org/wiki/National_identification_number)  
The Danish national identification is called a CPR-Number (from "Det **C**entrale **P**erson**r**egister")

### CPR-Number
The CPR-Number is no longer possible to validate by using the modulo 11 weigthed control since Denmark ran out of valid CPR-numbers on October 1, 2007.
The validation scheme was based on a weigthed Luhn algorithm with 1 control digits given  
```
    ddMMyysssk --> d1, d2, M1, M2, y3, y4, s1, s2, s3, k1
    k1 = 11 -  ((4 x d1 + 3 x d2 + 2 x M1 + 7 x M2 + 6 x y3 + 5 x y4 + 4 x s1 + 3 x s2, + 2 x s3) mod 11)
```
CPR-Number numbers starts with the persons birth-date in ddMMyy format, followed by the individual identification, which gives the century for the year portion following the table below, and gender based on 10'th digit being even for females and odd for males.
```
| 7'th digit | Years (y3, y4) | Century     |
| ---------- | -------------- | ----------- |
| 0 - 3      |                | 1900 - 1999 |
| 4          | < 37           | 2000 - 2036 |
| 4          | > 36           | 1937 - 1999 |
| 5 - 8      | < 58           | 2000 - 2057 |
| 5 - 8      | > 57           | 1858 - 1899 |
| 9          | > 36           | 1937 - 1999 |
| 9          | < 37           | 2000 - 2036 |
```
Even though not all CPR-Numbers is able to being validated, the modulo validation numbers is still assigned before assigning other numbers.