<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"><xsl:param name="IsInternal" select="1"></xsl:param>
	<xsl:output method="html"/>
	<xsl:template match="age_logindownline">
		<xsl:choose>
			<xsl:when test="@ip">
				<table class="tblDesc">
					<tr>
						<td class="colW1">{lbl:ip}</td>
						<td  class="colW2" >
							<xsl:value-of select="@ip" />
						</td>
                        <td class="colW1"></td>
                        <td  class="colW2" >
                            
                        </td>
                        <td class="colW1"></td>
                        <td  class="colW2" >

                        </td>
					</tr>
				</table>
			</xsl:when>
			<xsl:otherwise>
				<table class="tblDesc">
					<tr>
						<td class="colW1">
							<xsl:value-of select="rawdata" />
						</td>
                        <td  class="colW2" colspan="5">
                        </td>
                        <td class="colW1"></td>
                        <td  class="colW2" >

                        </td>
                        <td class="colW1"></td>
                        <td  class="colW2" >

                        </td>
					</tr>
				</table>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>
