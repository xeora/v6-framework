@Echo Off

Del Xeora.Web.RegularExpressions.il
Del Xeora.Web.RegularExpressions.dll.unsign

ildasm Xeora.Web.RegularExpressions.dll /out:Xeora.Web.RegularExpressions.il
ren Xeora.Web.RegularExpressions.dll Xeora.Web.RegularExpressions.dll.unsign
ilasm Xeora.Web.RegularExpressions.il /dll /key=Key.snk