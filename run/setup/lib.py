import os
import sys
import shutil
import var

def validate():
   if (len(sys.argv) < 2):
      show("project", var.EXAMPLE)
      sys.exit()
   elif sys.argv[1] == "--basic":   
      var.EX_SELECTS = 1
      var.DB_SELECTS = 1
      var.MQ_SELECTS = 0
      var.EV_SELECTS = 1
      var.PROJECT = "Basic"
      var.project = "basic"
      return False
   else: 
      recovery(sys.argv[1])   
      return True

def recovery(argument):
   if (argument == "--reset"):
      print("- recovering to original...")

      if not os.path.exists(var.CURRENT + "/.backup"):
         print("Invalid! There is no recovery")
         sys.exit()

      if os.path.exists(var.CURRENT + "/src"): 
         shutil.rmtree(var.CURRENT + "/src")

      if os.path.exists(var.CURRENT + "/run"): 
         shutil.rmtree(var.CURRENT + "/run")
         
      shutil.copytree(var.CURRENT + "/.backup/src", var.CURRENT + "/src")
      shutil.copytree(var.CURRENT + "/.backup/run", var.CURRENT + "/run")
      print("- repository is recovered!")
      sys.exit()

   if (argument.startswith("--")):
      print('invalid argument! Define a project name or type --reset for recovery')
      sys.exit()

   elif not os.path.exists(var.CURRENT + "/.backup"): 
      print("- preserving original repository in [.backup]...")
      os.mkdir(var.CURRENT + "/.backup")   
      shutil.copytree(var.CURRENT + "/src", var.CURRENT + "/.backup/src")
      shutil.copytree(var.CURRENT + "/run", var.CURRENT + "/.backup/run")   

def show(caption, content, option = 0):
   text = f"""
      {caption}
   ---------------------------------------------
   {content}"""
   if option != 0: return text
   else: return print(text)

def menu(caption, listing):
   listing = map(lambda x : f"   {x[0]} - {x[1]}", enumerate(listing))
   options = '\n'.join(listing)
   content = f"\n{options}\n\n{var.INPUTING_TEXT}"
   textual = show(caption, content, 1)
   return int(input((textual)) or 0)

def isAllowedFile(path):   
   _, name = os.path.split(path)
   if os.path.isdir(path): return False
   if name.startswith("."): return False
   if name.endswith(".py"): return False
   for ext in var.CHANGING_FILES:
      if name.endswith('.' + ext): 
         return True
   return False

def isAllowedFolder(path):
   name = os.path.basename(path)
   if not os.path.isdir(path): return False   
   if name.startswith("."): return False
   for ignore in var.IGNORE_FOLDERS:
      if ignore in name:
         return False
   return True

def looping(isFile, action, path = ".", text = ""):
   if text != "": print(text)   
   if path == ".": path = var.CURRENT
   if not isAllowedFolder(path): return
   for name in os.listdir(path):
      loop = os.path.join(path, name)
      if os.path.isdir(loop): looping(isFile, action, loop)
      if not isFile and isAllowedFolder(loop): action(loop)
      if isFile and isAllowedFile(loop): action(loop)

def loopingFiles(action, path = ".", text = ""):
   looping(True, action, path, text)

def loopingFolders(action, path = ".", text = ""):
   looping(False, action, path, text)

def removing(path):
   try:               
      print(f"- {relative(path)}")
      if os.path.isdir(path):
         shutil.rmtree(path)
      elif not os.path.isdir(path):
         os.remove(path)
   except:
      return

def renaming(path):   
   if os.path.isdir(path): name = os.path.basename(path)
   elif not os.path.isdir(path): _, name = os.path.split(path)   
   if "T3mplat3" in name: rename(path, "T3mplat3", var.PROJECT)
   if "t3mplat3" in name: rename(path, "t3mplat3", var.project)

def rename(oldpath, oldname, newname):   
   if oldname in oldpath:      
      print(f"- {relative(oldpath)}")
      newpath = oldpath.replace(oldname, newname)
      os.rename(oldpath, newpath)

def relative(path):
   return path.replace(var.CURRENT, ".")

def replacer(path, last, next = ""):
   if not os.path.isdir(path):
      with open(path, "r") as file:
         text = ''.join(file.readlines())
         file.close()
      if (last in text):
         print('- ' + relative(path))
         text = text.replace(last, next)
         with open(path, 'w') as file:
            file.write(text)
            file.close()

def rewriter(path):
   replacer(path, "T3mplat3", var.PROJECT)
   replacer(path, "t3mplat3", var.project)

def configureEX(option):
   if option == 0: return
   print("\n+ REMOVING SAMPLES")   

   if option == 1: discards=['Todo','Caixa','Lancamento']
   if option == 2: discards=['Account','Todo']
   if option == 3: discards=['Account','Todo','Caixa','Lancamento']
   
   removing(f"src/T3mplat3.Testing/Infrastructure")

   for term in discards:
      print(f"- {term}")
      removing(f"src/T3mplat3.Domain/{term}")   
      removing(f"src/T3mplat3.Testing/{term}")
      removing(f"src/T3mplat3.Infrastructure/Mappings/{term}Map.cs")
      removing(f"src/T3mplat3.Infrastructure/Repositories/{term}Repository.cs")
      removing(f"src/T3mplat3.Application/Controllers/{term}Controller.cs")
      replacer("src/T3mplat3.Domain/IUnitOfWork.cs", f"I{term}Repository {term}s {{ get; }}\n\n")
      replacer("src/T3mplat3.Application/Extensions/ServiceExtensions.cs", f"Add<{term}Handler>(services, testing);\n")
      replacer("src/T3mplat3.Infrastructure/UnitOfWork.cs", f"\n\n   public I{term}Repository {term}s => new {term}Repository(_provider);")
      replacer("src/T3mplat3.Infrastructure/Contexts/EfCoreContext.cs", f"public virtual DbSet<{term}>? {term}s {{ get; set; }}\n\n")      

   if "Account" in discards:
      replacer("src/T3mplat3.Testing/Factories.cs", var.CREATE_TEST_USER, "protected Login Createlogin(IUnitOfWork unitOfWork) => Login.Empty;")
      replacer("src/T3mplat3.Infrastructure/Settings.cs", "HasAuthentication => true", "HasAuthentication => false")

   elif not "Account" in discards:
      replacer("src/T3mplat3.Testing/Account/AbstractAccountSteps.cs", var.CLEAN_TODOS)

def configureDB(option):
   if (option == 0): return
   print("\n+ REMOVING ORMs")   

   selected = None
   discards = []
   contents = ['SqlServer', 'Sqlite', 'MySql', 'MongoDb', 'Postgres', 'Oracle']

   if option < 2: selected = contents[option]
   else: selected = contents[option-2]

   if option == 1: discards = ['Oracle','Postgres','MySql']
   else: discards = list(filter(lambda x : (x != selected), contents))

   removing("run/docker-compose.all.yml")

   for term in discards:
      print(f"- {term}")
      removing(f"src/T3mplat3.Infrastructure/Connections/{term}Connection")
      removing(f"run/docker-compose.db.{term.lower()}.yuml")
      replacer("run/docker-compose.yml", 'db?', selected)

def configureMQ(option):
   if (option == 0): return
   print("\n+ DEFINING MQs")
   discards = optMQ == 1 if ['rabbitmq'] else ['kafka']
   replaced = optMQ == 1 if 'app.UseKafka("test");' else 'app.UseRabbitMQ("test");'   

def configureEV(option):
   if (option == 0): return
   print("\n+ REDEFINING DTO")
   loopingFiles(lambda f : rename(f, "Command", "Request"))
   loopingFiles(lambda f : rename(f, "Query", "Request"))
   loopingFiles(lambda f : replacer(f, "Command", "Request"))
   loopingFiles(lambda f : replacer(f, "Query", "Request"))
   loopingFiles(lambda f : replacer(f, "\n\npublic interface IRequest : IRequest { }"))
   loopingFiles(lambda f : replacer(f, "Requestable", "Queryable"))
   loopingFiles(lambda f : replacer(f, "FromRequest", "FromQuery"))

def renameContents(): loopingFiles(rewriter, var.CURRENT, "\n+ RENAMING CONTENTS")      
def renameFolders(): loopingFolders(renaming, var.CURRENT, "\n+ RENAMING FOLDERS")
def renameFiles(): loopingFiles(renaming, var.CURRENT, "\n+ RENAMING FILES")

def removeFolders(path=None):
   if path == None:
      print("\n+ DROPPING BUILD FOLDERS")
      loopingFolders(removeFolders)
   elif not path == None:
      for name in var.REMOVE_FOLDERS:
         if path.endswith(name):                               
            removing(path)