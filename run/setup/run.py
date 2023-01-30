import os
import sys
import lib
import var

if lib.validate():
   var.EX_SELECTS = lib.menu(var.EX_CAPTION, var.EX_OPTIONS)
   var.DB_SELECTS = lib.menu(var.DB_CAPTION, var.DB_OPTIONS)
   var.MQ_SELECTS = lib.menu(var.MQ_CAPTION, var.MQ_OPTIONS)
   var.EV_SELECTS = lib.menu(var.EV_CAPTION, var.EV_OPTIONS)

lib.configureEX(var.EX_SELECTS)
lib.configureDB(var.DB_SELECTS)
lib.configureMQ(var.MQ_SELECTS)
lib.configureEV(var.EV_SELECTS)

lib.removeFolders()
lib.renameContents()
lib.renameFolders()
lib.renameFiles()

print("\n+ TESTING AND BUILDING...")

os.system('cd src && dotnet build & cd ..')
os.system("cd src && dotnet format & cd ..")
os.system('cd run && test & cd ..')