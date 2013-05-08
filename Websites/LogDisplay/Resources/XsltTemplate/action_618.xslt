<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_custgroupaddbyusername">
    <table class="tblDesc" style="width:200px;">
      <tr>
        <td class="colW1" style="width:100px;">{lbl:lblSystem}</td>
        <td class="colW2">
          <xsl:value-of select="@group" />
        </td>
      </tr>
    </table>

  </xsl:template>
</xsl:stylesheet>
