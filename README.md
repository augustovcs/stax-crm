# BMI Calculator with Personalized Recommendations 🧠💪

Este é um projeto simples em C# para calcular o Índice de Massa Corporal (IMC) de um usuário e fornecer recomendações personalizadas com base em uma base de dados local (`recommendationsdatabase.json`).

## 🧩 Funcionalidades

- Cálculo automático do IMC com base na altura e peso do usuário.
- Exibição do valor do IMC com duas casas decimais.
- Recomendação de categoria (ex: **Normal**, **Overweight**, **Obese**, etc).
- Sugestões personalizadas com base na faixa de IMC.
- Leitura de um banco de dados local em formato JSON.

---

## 📁 Estrutura do Projeto
📦 BMI_Calculator
├── Program.cs
├── Recommendations.cs
├── recommendationsdatabase.json
└── README.md

---

## 📐 Fórmula do IMC

O cálculo do IMC segue a fórmula padrão:


> Exemplo: uma pessoa com 70kg e 1.75m de altura terá um IMC de **22.86**.

---

## 🧠 Exemplo de Banco de Dados (`recommendationsdatabase.json`)
```json
[
  {
    "category": "Underweight",
    "bmiRange": "0-18.4",
    "advice": "Increase your calorie intake with nutrient-rich foods."
  },
  {
    "category": "Normal",
    "bmiRange": "18.5-24.9",
    "advice": "Keep up your healthy lifestyle!"
  },
  {
    "category": "Overweight",
    "bmiRange": "25-29.9",
    "advice": "Consider tracking your food with a nutrition app."
  },
  {
    "category": "Obese",
    "bmiRange": "30-100",
    "advice": "Talk to a healthcare professional about a weight loss plan."
  }
]
```
IMC = peso / (altura * altura)


> Exemplo: uma pessoa com 70kg e 1.75m de altura terá um IMC de **22.86**.

🚀 Como Rodar o Projeto
Clone este repositório:
```bash
git clone https://github.com/seunome/BMI_Calculator.git
cd BMI_Calculator
```

Compile o projeto:
```bash
dotnet build
```

Rode o projeto:
```bash
dotnet run
```

💡 Exemplo de Saída
```
BMI: 27.53kg
-----RECOMMENDATIONS-----
Category: Overweight
Advice: Consider tracking your food with a nutrition app.
-------------------------
```

🛠 Tecnologias Utilizadas
C# (.NET)
System.Text.Json
Console Application





