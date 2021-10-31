# Example usage
You can run the following command from this directory as long as you have [node](https://nodejs.org/) and npm (comes with node) installed.
```
npm i
node index.js ../examples/hello_world.bf
```

## Optional arguments
### --inputMode
- One of: number, char
- `--inputMode number` reads the input as a number
- `--inputMode char` reads the input as a utf8 character

### --outputMode
- One of: number, char
- `--outputMode number` writes the output as a number
- `--outputMode char` writes the output as a utf8 character

## implementation notes
- code counter can be negative.
- cells values are javascript Numbers, which can be much larger than a byte.