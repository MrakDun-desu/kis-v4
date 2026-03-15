const validationConstants = Object.freeze({
  maxNameLength: 30,
  maxNoteLength: 200,
  maxUnitNameLength: 10,
  maxDescriptionLength: 200,

  maxAllowedCost: 100_000,
  maxMarginPercent: 300,

  maxTransactionAmount: 100_000,
  maxCompositionAmount: 100_000,

  numberRegex: /^-?(?:0|[1-9]\d*)(?:\.\d+)?$/,
});

export default validationConstants;
