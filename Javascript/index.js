const fs = require('fs')
const path = require('path')
const args = require('args')
const Interpreter = require("./Interpreter")

args
  .option('inputMode', 'How input is read. One of [number, char]', 'char')
  .option('outputMode', 'How output is written. One of [number, char]', 'char')

const cli = () => {
  const flags = args.parse(process.argv)
  if (args.sub.length = 1) {
    runFromFile(args.sub[0], flags)
      .then((interpreter) => {
        console.log(interpreter.data)
        console.log('Program finished')
      })
  } else {

  }
}

const runFromFile = (filePath, flags) => {
  const fullPath = path.resolve(process.cwd(), filePath)
  const rawCode = fs.readFileSync(fullPath, { encoding: 'utf8' })
  return run(rawCode, flags)
}

const runFromStdin = () => {

}

const run = (rawCode, flags) => {
  const code = cleanCode(rawCode)
  const interpreter = new Interpreter(flags)
  interpreter.setCode(code)
  return interpreter.run()
    .then(() => {
      console.log()
      return interpreter
    })
  
}

const cleanCode = (code) => {
  return code
    .split('')
    .filter(char => Interpreter.codes.has(char))
}

module.exports = {
  runFromFile,
  run
}

if (require.main === module) {
  cli()
}