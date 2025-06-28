import html2pdf from "html2pdf.js";

export default {
	install(app) {
    const pdfDefaultMode = 'blob';
    const pdfType = "application/pdf";

    app.config.globalProperties.$utilities = {
      async generatePDF(htmlElement, pdfName
        , action = "download", mode = pdfDefaultMode) {
        const element = document.getElementById(htmlElement);

        if (!element) throw new Error(`Element ${htmlElement} not found`);

        const worker = html2pdf()
          .set({ filename: `${pdfName}.pdf` })
          .from(element);

        if (action === "download") {
          await worker.save();
          return;
        } else if (action === "file") {
          const blob = await worker.toPdf().outputPdf(mode);
          return new File([blob], `${pdfName}.pdf`, { type: pdfType });
        }

        throw new Error(`Acción ${action} no definida`);
      }
    };
	},
};