const { getNumber, getChar } = require("./inputUtils")

const defaultOptions = {
  inputMode: 'char',
  outputMode: 'char'
}

const inputModes = {
  'number': () => {
    return getNumber()
  },
  'char': () => {
    return getChar()
  }
}

const outputModes = {
  'number': (num) => num.toString(),
  'char': (num) => String.fromCharCode(num)
}

class Interpreter {
  constructor (options = defaultOptions) {
    this.inputFn = options.inputMode instanceof Function
      ? options.inputMode
      : (
        inputModes[options.inputMode]
        || inputModes.number
      )
    this.outputFn = options.outputMode instanceof Function
      ? options.outputMode
      : (
        outputModes[options.outputMode]
        || outputModes.number
      )
    
    this.actions = {
      '>': this.tapeForward,
      '<': this.tapeBackward,
      '+': this.increment,
      '-': this.decrement,
      '.': this.output,
      ',': this.input,
      '[': this.jumpForwardIfZero,
      ']': this.jumpBackwardIfNotZero
    }
    
    this.reset()
  }

  /**
   * @param {string[]} code - a list of bf char codes. One of: ['>', '<', '+', '-', '.', ',', '[', ']']
   */
  setCode = (code) => {
    this.code = code
    this.jumpMap = this.createJumpMap()
  }

  createJumpMap = (code = this.code) => {
    const stack = []
    const jumpMap = {}

    for(let i=0; i < code.length; i++)  {
      if (code[i] === '[') {
        stack.push(i)
      } else if (code[i] === ']') {
        if(stack.length !== 0) {
          const startIndex = stack.pop()
          const endIndex = i
          jumpMap[startIndex] = endIndex+1
          jumpMap[endIndex] = startIndex+1
        }
      }
    }

    return jumpMap
  }

  reset = () => {
    this.codeIndex = 0
    this.dataIndex = 0
    this.data = new Map()
  }

  run = async () => {
    while(this.codeIndex < this.code.length) {
      const charCode = this.code[this.codeIndex]
      await this.doAction(charCode)
    }
  }

  doAction = (charCode) => {
    const action = this.actions[charCode]

    if (action && action instanceof Function) {
      return action()
    }
  }

  currentData = () => {
    return this.data.get(this.dataIndex) || 0
  }

  tapeForward = () => {
    this.dataIndex++
    this.codeIndex++
  }

  tapeBackward = () => {
    this.dataIndex--
    this.codeIndex++
  }

  increment = () => {
    this.data.set(this.dataIndex, this.currentData() + 1)
    this.codeIndex++
  }

  decrement = () => {
    this.data.set(this.dataIndex, this.currentData() - 1)
    this.codeIndex++
  }

  output = () => {
    const output = this.outputFn(this.currentData())
    process.stdout.write(output)
    this.codeIndex++
  }

  input = async () => {
    const input = await this.inputFn()
    this.data.set(this.dataIndex, input)
    this.codeIndex++
  }

  jumpForwardIfZero = () => {
    if(this.currentData() === 0) {
      this.codeIndex = this.getJump()
    } else {
      this.codeIndex++
    }
  }

  jumpBackwardIfNotZero = () => {
    if(this.currentData() !== 0) {
      this.codeIndex = this.getJump()
    } else {
      this.codeIndex++
    }
  }

  getJump = (index = this.codeIndex, jumpMap = this.jumpMap) => {
    const nextIndex = jumpMap[index]
    return nextIndex === undefined
      ? index + 1
      : nextIndex
  }
}

Interpreter.codes = new Set(['>', '<', '+', '-', '.', ',', '[', ']'])

module.exports = Interpreter