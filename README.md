# Ether.TextAnalysis

Docs coming soon. For now, just know this:

- Download this rather large zip: http://nlp.stanford.edu/software/stanford-corenlp-full-2015-12-09.zip
- In that zip, find the models jar: stanford-corenlp-3.6.0-models.jar
- Put this somewhere like G:/StanfordModels/ and unarch it. It will be 420MB. Sorry. 
- When you consume this library, you need to make an AppSetting called CoreNLP.ModelDirectory. This should be the path to the folder where you put the models, for instance: 

  <appSettings>
    <add key="CoreNLP.ModelDirectory" value="G:\StanfordModels"/>    
  </appSettings>
