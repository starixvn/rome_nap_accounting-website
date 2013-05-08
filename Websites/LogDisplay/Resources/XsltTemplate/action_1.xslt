<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"><xsl:param name="IsInternal" select="1"></xsl:param>
	<xsl:output method="html"/>	
	<xsl:template match="Acc_CustomerUpdDisableFI">
		<table width="100%" class="tblDesc">
			<tr>
				<td>
					<b>
						{lbl:disabledfi}: <xsl:value-of select="@DisableFinancial" />
						&#160;
						{lbl:disabledownline}: <xsl:value-of select="@DisableDownline" />
						&#160;
						{lbl:subaccount}: <xsl:value-of select="@SubAccName" />
						&#160;
						{lbl:lang}: <xsl:value-of select="@Language" />
						&#160;
						IP: <xsl:value-of select="@IP" />
						&#160;
					</b>
				</td>
			</tr>			
		</table>		
	</xsl:template>
</xsl:stylesheet>